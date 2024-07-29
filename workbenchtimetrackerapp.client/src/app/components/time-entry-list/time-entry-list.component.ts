import { Component, OnInit, OnDestroy } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { TimeEntry } from '../../models/time-entry.model';
import { TimeEntryService } from '../../services/time-entry.service';
import { SharedService } from '../../services/shared.service';
import { TimeEntryFormComponent } from '../time-entry-form/time-entry-form.component';
import { PeopleService } from '../../services/people.service';
import { WorkTaskService } from '../../services/work-task.service';
import { Person } from '../../models/person.model';
import { WorkTask } from '../../models/work-task.model';

@Component({
  selector: 'app-time-entry-list',
  templateUrl: './time-entry-list.component.html',
  styleUrls: ['./time-entry-list.component.css']
})
export class TimeEntryListComponent implements OnInit, OnDestroy {
  timeEntries: TimeEntry[] = []; // List of time entries for the selected person
  personSubscription: Subscription = new Subscription(); // Subscription to track selected person changes
  selectedPersonId: number | null = null; // ID of the currently selected person
  personNames: { [key: number]: string } = {}; // Cache for person names
  taskNames: { [key: number]: string } = {}; // Cache for work task names

  constructor(
    private timeEntryService: TimeEntryService,
    private modalService: NgbModal,
    private sharedService: SharedService,
    private peopleService: PeopleService,
    private workTaskService: WorkTaskService
  ) { }

  ngOnInit(): void {
    // Subscribe to the selected person and load time entries when the person changes
    this.personSubscription = this.sharedService.selectedPerson$.subscribe(person => {
      if (person && person.id !== undefined) {
        this.selectedPersonId = person.id;
        this.loadTimeEntries(person.id);
      } else {
        this.selectedPersonId = null;
        this.timeEntries = [];
      }
    });
  }

  ngOnDestroy(): void {
    // Clean up subscription to avoid memory leaks
    this.personSubscription.unsubscribe();
  }

  loadTimeEntries(personId: number): void {
    // Fetch time entries for the selected person
    this.timeEntryService.getTimeEntriesByPersonId(personId).subscribe(entries => {
      this.timeEntries = entries.filter(entry => entry.personId === personId);
      this.loadNames();
    });
  }

  loadNames(): void {
    // Load and cache person and task names
    this.timeEntries.forEach(entry => {
      if (!this.personNames[entry.personId]) {
        this.peopleService.getPerson(entry.personId).subscribe(person => {
          this.personNames[entry.personId] = person.fullName;
        });
      }
      if (!this.taskNames[entry.workTaskId]) {
        this.workTaskService.getWorkTask(entry.workTaskId).subscribe(task => {
          this.taskNames[entry.workTaskId] = task.name;
        });
      }
    });
  }

  getPersonName(personId: number): string {
    // Return cached person name or 'Loading...' if not available
    return this.personNames[personId] || 'Loading...';
  }

  getWorkTaskName(workTaskId: number): string {
    // Return cached work task name or 'Loading...' if not available
    return this.taskNames[workTaskId] || 'Loading...';
  }

  addEntry(): void {
    if (this.selectedPersonId !== null) {
      // Open modal to add a new time entry
      const modalRef = this.modalService.open(TimeEntryFormComponent);
      modalRef.componentInstance.timeEntry = { entryDateTime: new Date(), personId: this.selectedPersonId, workTaskId: 0 } as TimeEntry;
      modalRef.result.then((result) => {
        if (result) {
          this.timeEntryService.addTimeEntry(result).subscribe((newEntry) => {
            // Add the new entry to the list and update names
            this.timeEntries.push(newEntry);
            this.loadNames();
          });
        }
      }).catch((error) => {
        console.log('Modal dismissed', error);
      });
    }
  }

  editEntry(entry: TimeEntry): void {
    // Open modal to edit an existing time entry
    const modalRef = this.modalService.open(TimeEntryFormComponent);
    modalRef.componentInstance.timeEntry = { ...entry }; // Pass a copy of the entry to avoid modifying the original
    modalRef.result.then((result) => {
      if (result) {
        if (result.id !== entry.id) {
          console.error('Mismatched ID during update');
          return;
        }
        this.timeEntryService.updateTimeEntry(result).subscribe(() => {
          if (this.selectedPersonId !== null) {
            this.loadTimeEntries(this.selectedPersonId); // Reload time entries after update
          }
        });
      }
    }).catch((error) => {
      console.log('Modal dismissed', error);
    });
  }

  deleteEntry(id?: number): void {
    if (id !== undefined) {
      // Confirm and delete the time entry
      if (confirm('Are you sure you want to delete this entry?')) {
        this.timeEntryService.deleteTimeEntry(id).subscribe(() => {
          this.timeEntries = this.timeEntries.filter(entry => entry.id !== id); // Remove deleted entry from the list
        });
      }
    } else {
      console.error('Attempted to delete entry with undefined id');
    }
  }
}

import { Component, OnInit, OnDestroy } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { TimeEntry } from '../../models/time-entry.model';
import { TimeEntryService } from '../../services/time-entry.service';
import { SharedService } from '../../services/shared.service';
import { TimeEntryFormComponent } from '../time-entry-form/time-entry-form.component';

@Component({
  selector: 'app-time-entry-list',
  templateUrl: './time-entry-list.component.html',
  styleUrls: ['./time-entry-list.component.css']
})
export class TimeEntryListComponent implements OnInit, OnDestroy {
  timeEntries: TimeEntry[] = []; // List of time entries to display
  personSubscription: Subscription = new Subscription(); // Subscription to the shared service for person changes
  selectedPersonId: number | null = null; // Currently selected person's ID

  constructor(
    private timeEntryService: TimeEntryService, // Service for time entry operations
    private modalService: NgbModal, // Service for handling modals
    private sharedService: SharedService // Service for shared state, including selected person
  ) { }

  ngOnInit(): void {
    // Subscribe to the selected person observable from the shared service
    this.personSubscription = this.sharedService.selectedPerson$.subscribe(person => {
      if (person && person.id !== undefined) {
        this.selectedPersonId = person.id; // Set the selected person's ID
        this.loadTimeEntries(person.id); // Load time entries for the selected person
      } else {
        this.selectedPersonId = null; // No person selected
        this.timeEntries = []; // Clear the time entries list
      }
    });
  }

  ngOnDestroy(): void {
    // Unsubscribe from the person observable to prevent memory leaks
    this.personSubscription.unsubscribe();
  }

  loadTimeEntries(personId: number): void {
    // Fetch time entries for the given person ID
    this.timeEntryService.getTimeEntriesByPersonId(personId).subscribe(entries => {
      this.timeEntries = entries; // Update the time entries list
    });
  }

  addEntry(): void {
    if (this.selectedPersonId !== null) {
      // Open the TimeEntryFormComponent modal for adding a new entry
      const modalRef = this.modalService.open(TimeEntryFormComponent);
      modalRef.componentInstance.timeEntry = { entryDateTime: new Date(), personId: this.selectedPersonId, workTaskId: 0 } as TimeEntry;

      modalRef.result.then((result) => {
        if (result) {
          // Add the new time entry if modal result is successful
          this.timeEntryService.addTimeEntry(result).subscribe((newEntry) => {
            // Refresh the time entries list
            this.loadTimeEntries(this.selectedPersonId as number); 
          });
        }
      }).catch((error) => {
        console.log('Modal dismissed', error); 
      });
    }
  }

  editEntry(entry: TimeEntry): void {
    // Open the TimeEntryFormComponent modal for editing an existing entry
    const modalRef = this.modalService.open(TimeEntryFormComponent);
    modalRef.componentInstance.timeEntry = { ...entry }; // Pass the entry to edit

    modalRef.result.then((result) => {
      if (result) {
        if (result.id !== entry.id) {
          console.error('Mismatched ID during update'); 
          return;
        }
        // Update the time entry if modal result is successful
        this.timeEntryService.updateTimeEntry(result).subscribe(() => {
          // Refresh the time entries list
          this.loadTimeEntries(this.selectedPersonId as number); 
        });
      }
    }).catch((error) => {
      console.log('Modal dismissed', error); 
    });
  }

  deleteEntry(id?: number): void {
    if (id !== undefined) {
      if (confirm('Are you sure you want to delete this entry?')) {
        // Confirm and delete the time entry if confirmed
        this.timeEntryService.deleteTimeEntry(id).subscribe(() => {
          // Refresh the time entries list
          this.loadTimeEntries(this.selectedPersonId as number);
        });
      }
    } else {
      console.error('Attempted to delete entry with undefined id'); 
    }
  }
}

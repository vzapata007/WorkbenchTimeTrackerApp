import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TimeEntry } from '../../models/time-entry.model';
import { Person } from '../../models/person.model';
import { WorkTask } from '../../models/work-task.model';
import { PeopleService } from '../../services/people.service';
import { WorkTaskService } from '../../services/work-task.service';

@Component({
  selector: 'app-time-entry-form',
  templateUrl: './time-entry-form.component.html',
  styleUrls: ['./time-entry-form.component.css']
})
export class TimeEntryFormComponent implements OnInit {
  @Input() timeEntry?: TimeEntry; // Optional input for time entry data
  people: Person[] = []; // List of people  
  workTasks: WorkTask[] = []; // List of work tasks
  timeEntryForm: FormGroup; // Reactive form for time entry
  errorMessage: string = ''; // Error message to display

  constructor(  
    public activeModal: NgbActiveModal,
    private peopleService: PeopleService,
    private workTaskService: WorkTaskService,
    private fb: FormBuilder
  ) {
    // Initialize form with required fields
    this.timeEntryForm = this.fb.group({
      personId: [{ value: '', disabled: true }], // personId is read-only
      workTaskId: ['', Validators.required],
      entryDateTime: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadPeopleAndTasks(); // Load people and tasks on initialization
  }

  // Load people and work tasks from services
  private loadPeopleAndTasks(): void {
    this.peopleService.getPeople().subscribe(people => {
      this.people = people;
      this.initializeForm(); // Initialize form with loaded data
    });

    this.workTaskService.getWorkTasks().subscribe(tasks => {
      this.workTasks = tasks;
      this.initializeForm(); // Initialize form with loaded data
    });
  }

  // Initialize form with existing time entry data if available
  private initializeForm(): void {
    if (this.timeEntry) {
      this.timeEntryForm.patchValue({
        personId: this.timeEntry.personId || '', // Ensure personId is a string
        workTaskId: this.timeEntry.workTaskId || '', // Ensure workTaskId is a string
        entryDateTime: this.timeEntry.entryDateTime ? this.formatDateForInput(this.timeEntry.entryDateTime) : this.formatDateForInput(new Date())
      });
    }
  }

  // Format date to match input type="datetime-local" format
  private formatDateForInput(date: string | Date): string {
    return new Date(date).toISOString().slice(0, 16); // Format: yyyy-mm-ddThh:mm
  }

  // Save form data and close the modal
  save(): void {
    this.errorMessage = '';

    if (this.timeEntryForm.invalid) {
      this.timeEntryForm.markAllAsTouched(); // Mark all fields as touched to show validation errors

      // Show specific validation errors
      const { workTaskId, entryDateTime } = this.timeEntryForm.controls;

      if (workTaskId.invalid) {
        this.errorMessage = 'Work Task is required.';
      } else if (entryDateTime.invalid) {
        this.errorMessage = 'Entry Date Time is required.';
      }

      return;
    }

    const { workTaskId: workTaskIdString, entryDateTime } = this.timeEntryForm.value;
    const workTaskId = Number(workTaskIdString);


    // Update time entry with valid form values
    this.timeEntry = {
      ...this.timeEntry!,
      workTaskId,
      entryDateTime: new Date(entryDateTime) // Ensure entryDateTime is a Date object
    };

    // Pass updated time entry back to the parent component
    this.activeModal.close(this.timeEntry);
  }

  // Dismiss the modal without saving changes
  cancel(): void {
    this.activeModal.dismiss();
  }
}

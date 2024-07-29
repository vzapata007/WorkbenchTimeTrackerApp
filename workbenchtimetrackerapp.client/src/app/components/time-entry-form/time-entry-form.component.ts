import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TimeEntry } from '../../models/time-entry.model';
import { PeopleService } from '../../services/people.service';
import { WorkTaskService } from '../../services/work-task.service';
import { Person } from '../../models/person.model';
import { WorkTask } from '../../models/work-task.model';

@Component({
  selector: 'app-time-entry-form',
  templateUrl: './time-entry-form.component.html',
  styleUrls: ['./time-entry-form.component.css']
})
export class TimeEntryFormComponent implements OnInit {
  @Input() timeEntry!: TimeEntry; 
  people: Person[] = []; 
  workTasks: WorkTask[] = []; 

  constructor(
    public activeModal: NgbActiveModal, // Service to manage modal interactions
    private peopleService: PeopleService, // Service to fetch people data
    private workTaskService: WorkTaskService // Service to fetch work tasks data
  ) { }

  ngOnInit(): void {
    this.loadPeopleAndTasks(); // Load people and tasks when the component initializes
  }

  loadPeopleAndTasks(): void {
    // Fetch people data from the service
    this.peopleService.getPeople().subscribe((people: Person[]) => {
      this.people = people; 
    });

    // Fetch work tasks data from the service
    this.workTaskService.getWorkTasks().subscribe((tasks: WorkTask[]) => {
      this.workTasks = tasks; 
      this.initializeForm(); 
    });
  }

  initializeForm(): void {
    // Set the current date and time if not already set in the timeEntry
    if (!this.timeEntry.entryDateTime) {
      this.timeEntry.entryDateTime = new Date(); // Default to current date and time
    }

    // Set the first work task as default if no task is selected
    if (this.workTasks.length > 0 && !this.timeEntry.workTaskId) {
      this.timeEntry.workTaskId = this.workTasks[0].id!; // Set the first available task as default
    }
  }

  save(): void {
    // Find the selected person and work task based on IDs from the timeEntry
    const selectedPerson = this.people.find(person => person.id === this.timeEntry.personId);
    const selectedTask = this.workTasks.find(task => task.id === this.timeEntry.workTaskId);

    // Update the timeEntry with the full name of the selected person
    if (selectedPerson) {
      this.timeEntry.personFullName = selectedPerson.fullName;
    }

    // Update the timeEntry with the name of the selected work task
    if (selectedTask) {
      this.timeEntry.workTaskName = selectedTask.name;
    }

    // Close the modal and pass the updated timeEntry back to the parent component
    this.activeModal.close(this.timeEntry);
  }

  cancel(): void {
    // Dismiss the modal without saving changes
    this.activeModal.dismiss();
  }
}

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
    public activeModal: NgbActiveModal,
    private peopleService: PeopleService,
    private workTaskService: WorkTaskService
  ) { }

  ngOnInit(): void {
    this.loadPeopleAndTasks();
  }

  loadPeopleAndTasks(): void {
    this.peopleService.getPeople().subscribe((people: Person[]) => {
      this.people = people;
    });

    this.workTaskService.getWorkTasks().subscribe((tasks: WorkTask[]) => {
      this.workTasks = tasks;
      this.initializeForm(); // Initialize form after tasks are loaded
    });
  }

  initializeForm(): void {
    // Set the current date and time if not already set
    if (!this.timeEntry.entryDateTime) {
      this.timeEntry.entryDateTime = new Date();
    }

    // Set the first task as default if no task is selected
    if (this.workTasks.length > 0 && !this.timeEntry.workTaskId) {
      this.timeEntry.workTaskId = this.workTasks[0].id!;
    }
  }

  save(): void {
    this.activeModal.close(this.timeEntry);
  }

  cancel(): void {
    this.activeModal.dismiss();
  }
}

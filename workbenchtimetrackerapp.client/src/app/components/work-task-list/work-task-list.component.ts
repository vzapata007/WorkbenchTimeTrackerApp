import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { SharedService } from '../../services/shared.service';
import { Person } from '../../models/person.model';
import { WorkTask } from '../../models/work-task.model';

@Component({
  selector: 'app-work-task-list',
  templateUrl: './work-task-list.component.html',
  styleUrls: ['./work-task-list.component.css']
})
export class WorkTaskListComponent implements OnInit, OnDestroy {
  person: Person | null = null; // Selected person from the shared service
  workTasks: WorkTask[] = []; // List of work tasks associated with the selected person
  private personSubscription: Subscription = new Subscription(); // Subscription to track person changes
  private workTasksSubscription: Subscription = new Subscription(); // Subscription to track work task changes

  constructor(private sharedService: SharedService) { }

  ngOnInit(): void {
    // Subscribe to selected person changes from the shared service
    this.personSubscription = this.sharedService.selectedPerson$.subscribe(person => {
      this.person = person;
      // Additional logic to load work tasks can be added here if needed
    });
  }

  ngOnDestroy(): void {
    // Clean up subscriptions to avoid memory leaks
    this.personSubscription.unsubscribe();
    this.workTasksSubscription.unsubscribe();
  }
}

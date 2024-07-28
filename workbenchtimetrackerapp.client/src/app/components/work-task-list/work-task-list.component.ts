import { Component, OnInit } from '@angular/core';
import { WorkTaskService } from '../../services/work-task.service';

@Component({
  selector: 'app-work-task-list',
  templateUrl: './work-task-list.component.html',
  styleUrls: ['./work-task-list.component.css']
})
export class WorkTaskListComponent implements OnInit {
  workTasks: any[] = [];

  constructor(private workTaskService: WorkTaskService) { }

  ngOnInit(): void {
    this.workTaskService.getWorkTasks().subscribe(data => {
      this.workTasks = data;
    });
  }
}

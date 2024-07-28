import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

interface Person {
  id: number;
  fullName: string;
}

interface WorkTask {
  id: number;
  name: string;
}

@Component({
  selector: 'app-time-entry-form',
  templateUrl: './time-entry-form.component.html',
  styleUrls: ['./time-entry-form.component.css']
})
export class TimeEntryFormComponent implements OnInit {
  public people: Person[] = [];
  public workTasks: WorkTask[] = [];
  public timeEntry = {
    entryDateTime: '',
    personId: null,
    workTaskId: null
  };

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getPeople();
    this.getWorkTasks();
  }

  getPeople() {
    this.http.get<Person[]>('/api/People').subscribe(
      (result) => {
        this.people = result;
      },
      (error) => {
        console.error('Error fetching people:', error);
      }
    );
  }

  getWorkTasks() {
    this.http.get<WorkTask[]>('/api/WorkTasks').subscribe(
      (result) => {
        this.workTasks = result;
      },
      (error) => {
        console.error('Error fetching work tasks:', error);
      }
    );
  }

  onSubmit() {
    this.http.post('/api/TimeEntries', this.timeEntry).subscribe(
      (response) => {
        console.log('Time entry created:', response);
        // Optionally reset form or navigate to another view
      },
      (error) => {
        console.error('Error creating time entry:', error);
      }
    );
  }
}

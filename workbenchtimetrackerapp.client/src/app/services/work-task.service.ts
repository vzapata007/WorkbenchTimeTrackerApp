import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { WorkTask } from '../models/work-task.model';

@Injectable({
  providedIn: 'root'
})
export class WorkTaskService {
  private apiUrl = '/api/WorkTasks'; // URL to the API endpoint for work tasks

  constructor(private http: HttpClient) { }

  // Get a list of all work tasks
  getWorkTasks(): Observable<WorkTask[]> {
    return this.http.get<WorkTask[]>(this.apiUrl);
  }

  // Get a specific work task by ID
  getWorkTask(id: number): Observable<WorkTask> {
    return this.http.get<WorkTask>(`${this.apiUrl}/${id}`);
  }
}

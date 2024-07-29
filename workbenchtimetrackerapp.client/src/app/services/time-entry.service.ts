import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { TimeEntry } from '../models/time-entry.model';
import { Person } from '../models/person.model';

@Injectable({
  providedIn: 'root'
})
export class TimeEntryService {
  private apiUrlPeople = '/api/People'; // URL to the API endpoint for people
  private apiUrl = '/api/TimeEntries'; // URL to the API endpoint for time entries

  constructor(private http: HttpClient) { }

  // Get time entries for a specific person by their ID
  getTimeEntriesByPersonId(personId: number): Observable<TimeEntry[]> {
    return this.http.get<Person>(`${this.apiUrlPeople}/${personId}`).pipe(
      map(person =>
        person.workTasks?.flatMap(task => task.timeEntries).filter((entry): entry is TimeEntry => !!entry) ?? []
      )
    );
  }

  // Add a new time entry
  addTimeEntry(timeEntry: TimeEntry): Observable<TimeEntry> {
    return this.http.post<TimeEntry>(this.apiUrl, timeEntry);
  }

  // Update an existing time entry
  updateTimeEntry(timeEntry: TimeEntry): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${timeEntry.id}`, timeEntry);
  }

  // Delete a time entry by its ID
  deleteTimeEntry(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}

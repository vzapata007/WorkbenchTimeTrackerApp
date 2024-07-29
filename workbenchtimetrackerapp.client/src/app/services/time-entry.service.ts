import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TimeEntry } from '../models/time-entry.model';

@Injectable({
  providedIn: 'root'
})
export class TimeEntryService {
  private apiUrl = '/api/TimeEntries'; // URL to the API endpoint for time entries

  constructor(private http: HttpClient) { }

  getTimeEntriesByPersonId(personId: number): Observable<TimeEntry[]> {
    return this.http.get<TimeEntry[]>(`${this.apiUrl}/person/${personId}`);
  }

  addTimeEntry(timeEntry: TimeEntry): Observable<TimeEntry> {
    return this.http.post<TimeEntry>(this.apiUrl, timeEntry);
  }

  updateTimeEntry(timeEntry: TimeEntry): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${timeEntry.id}`, timeEntry);
  }

  deleteTimeEntry(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}

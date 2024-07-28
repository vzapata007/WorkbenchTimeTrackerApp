import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TimeEntryService {
  private apiUrl = '/api/TimeEntries';

  constructor(private http: HttpClient) { }

  getTimeEntries(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  addTimeEntry(timeEntry: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, timeEntry);
  }
}

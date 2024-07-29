import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Person } from '../models/person.model';
import { TimeEntry } from '../models/time-entry.model';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  private selectedPersonSource = new BehaviorSubject<Person | null>(null); // Subject to hold the currently selected person
  selectedPerson$ = this.selectedPersonSource.asObservable(); // Observable to expose the selected person
  private apiUrl = '/api/People'; // URL to the API endpoint for people

  constructor(private http: HttpClient) { }

  // Set the currently selected person
  selectPerson(person: Person): void {
    this.selectedPersonSource.next(person);
  }

  // Get a list of all people
  getPeople(): Observable<Person[]> {
    return this.http.get<Person[]>(this.apiUrl);
  }

  // Get a specific person by their ID
  getPerson(id: number): Observable<Person> {
    return this.http.get<Person>(`${this.apiUrl}/${id}`);
  }

  // Get time entries for a specific person by their ID
  getTimeEntriesForPerson(personId: number): Observable<TimeEntry[]> {
    return this.http.get<TimeEntry[]>(`${this.apiUrl}/${personId}/time-entries`);
  }
}

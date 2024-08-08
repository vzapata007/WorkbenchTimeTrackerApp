import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { PersonDTO } from './../model/personDTO';
import { TimeEntryDTO } from './../model/timeEntryDTO';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  private selectedPersonSource = new BehaviorSubject<PersonDTO | null>(null); // Subject to hold the currently selected person
  selectedPerson$ = this.selectedPersonSource.asObservable(); // Observable to expose the selected person
  private apiUrl = '/api/People'; // URL to the API endpoint for people

  constructor(private http: HttpClient) { }

  // Set the currently selected person
  selectPerson(person: PersonDTO): void {
    this.selectedPersonSource.next(person);
  }

  // Get a list of all people
  getPeople(): Observable<PersonDTO[]> {
    return this.http.get<PersonDTO[]>(this.apiUrl);
  }

  // Get a specific person by their ID
  getPerson(id: number): Observable<PersonDTO> {
    return this.http.get<PersonDTO>(`${this.apiUrl}/${id}`);
  }

  // Get time entries for a specific person by their ID
  getTimeEntriesForPerson(personId: number): Observable<TimeEntryDTO[]> {
    return this.http.get<TimeEntryDTO[]>(`${this.apiUrl}/${personId}/time-entries`);
  }
}

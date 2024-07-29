import { Component, OnInit } from '@angular/core';
import { PeopleService } from '../../services/people.service';
import { Person } from '../../models/person.model';
import { SharedService } from '../../services/shared.service';

@Component({
  selector: 'app-people-list',
  templateUrl: './people-list.component.html',
  styleUrls: ['./people-list.component.css']
})
export class PeopleListComponent implements OnInit {
  people: Person[] = []; // List of people to display
  errorMessage: string = ''; // Error message for display in case of failure

  constructor(
    private peopleService: PeopleService, // Service to fetch people data
    private sharedService: SharedService // Service to communicate selected person
  ) { }

  ngOnInit(): void {
    this.loadPeople(); // Load people when component initializes
  }

  loadPeople(): void {
    this.peopleService.getPeople().subscribe({
      next: (data: Person[]) => {
        this.people = data; // Set the people list with fetched data
        if (this.people.length > 0) {
          this.selectPerson(this.people[0]); // Select the first person by default
        }
      },
      error: (error: any) => {
        console.error('Error loading people:', error); // Log error to console
        this.errorMessage = 'Error loading people'; // Set error message for display
      }
    });
  }

  selectPerson(person: Person): void {
    this.sharedService.selectPerson(person); // Notify shared service of the selected person
  }
}

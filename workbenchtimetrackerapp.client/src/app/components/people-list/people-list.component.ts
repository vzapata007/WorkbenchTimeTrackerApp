import { Component, OnInit } from '@angular/core';
import { PeopleService } from '../../api/api/people.service';
//import { PeopleService } from '../../services/people.service';
//import { Person } from '../../models/person.model';
import { PersonDTO } from '../../api/model/personDTO';
import { SharedService } from '../../api/api/shared.service';

@Component({
  selector: 'app-people-list',
  templateUrl: './people-list.component.html',
  styleUrls: ['./people-list.component.css']
})
export class PeopleListComponent implements OnInit {
  people: PersonDTO[] = [];
  errorMessage: string = '';

  constructor(
    private peopleService: PeopleService, // Service for fetching people
    private sharedService: SharedService // Service for shared state
  ) { }

  ngOnInit(): void {
    this.loadPeople(); // Load people when component initializes
  }

  private loadPeople(): void {
    this.peopleService.apiPeopleGet().subscribe({
      next: (data) => {
        this.people = data;
        if (data.length) {
          this.selectPerson(data[0]); // Default to the first person if available
        }
      },
      error: (err) => {
        console.error('Error loading people:', err);
        this.errorMessage = 'Error loading people';
      }
    });
  }

  selectPerson(person: PersonDTO): void {
    this.sharedService.selectPerson(person); // Notify shared service
  }
}

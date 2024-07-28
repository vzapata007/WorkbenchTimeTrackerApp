import { Component, OnInit } from '@angular/core';
import { PeopleService } from '../../services/people.service';
import { Person } from '../../models/person.model';


@Component({
  selector: 'app-people-list',
  templateUrl: './people-list.component.html',
  styleUrls: ['./people-list.component.css']
})
export class PeopleListComponent implements OnInit {

  people: Person[] = [];
  errorMessage: string = '';

  constructor(private peopleService: PeopleService) { }

  ngOnInit(): void {
    this.loadPeople();
  }

  loadPeople(): void {
    this.peopleService.getPeople().subscribe({
      next: (data: Person[]) => {
        this.people = data;
      },
      error: (error: any) => {
        console.error('Error loading people:', error);
        this.errorMessage = 'Error loading people';
      }
    });
  }
}

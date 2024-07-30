import { Component, OnInit } from '@angular/core';
import { SharedService } from './services/shared.service';
import { Person } from './models/person.model';
import { PeopleService } from './services/people.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Workbench Time Tracker';
  people: Person[] = [];
  selectedPersonId: number | null = null;

  constructor(
    private peopleService: PeopleService, // Service for fetching people
    private sharedService: SharedService // Service for shared state
  ) { }

  ngOnInit(): void {
    this.loadPeople(); // Load people when component initializes
  }

  loadPeople(): void {
    this.peopleService.getPeople().subscribe({
      next: (people) => {
        this.people = people;
        if (people.length) {
          this.selectPerson(people[0].id!); // Select the first person if available
        }
      },
      error: (err) => console.error('Error fetching people:', err)
    });
  }

  onPersonSelect(event: Event): void {
    const personId = Number((event.target as HTMLSelectElement).value);
    if (!isNaN(personId)) {
      this.selectPerson(personId); // Select the person by ID
    }
  }

  private selectPerson(personId: number): void {
    this.peopleService.getPerson(personId).subscribe({
      next: (person) => {
        this.selectedPersonId = personId;
        this.sharedService.selectPerson(person); // Update shared service
      },
      error: (err) => console.error('Error fetching person:', err)
    });
  }
}

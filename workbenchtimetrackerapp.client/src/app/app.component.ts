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
  title = 'workbenchtimetrackerapp.client'; 
  people: Person[] = []; 
  selectedPerson: Person | null = null; 

  constructor(private peopleService: PeopleService, private sharedService: SharedService) { }

  ngOnInit() {
    this.loadPeople(); 
  }

  // Load the list of people from the PeopleService
  loadPeople() {
    this.peopleService.getPeople().subscribe(
      (data) => {
        this.people = data; 
        // Select the first person by default if available
        if (this.people.length > 0) {
          this.selectPerson(this.people[0].id!);
        }
      },
      (error) => {
        console.error('Error fetching people:', error); 
      }
    );
  }

  // Handle selection change in the dropdown
  onPersonSelect(event: Event) {
    const target = event.target as HTMLSelectElement;
    const personId = Number(target.value);
    if (!isNaN(personId)) {
      this.selectPerson(personId); // Select the person with the chosen ID
    }
  }

  // Select a person by ID and notify the SharedService
  selectPerson(personId: number) {
    this.peopleService.getPerson(personId).subscribe(
      (person) => {
        this.selectedPerson = person; 
        this.sharedService.selectPerson(person); 
      },
      (error) => {
        console.error('Error fetching person:', error); 
      }
    );
  }
}

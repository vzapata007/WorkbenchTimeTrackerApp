import { Component, OnInit } from '@angular/core';
import { TimeEntryService } from '../../services/time-entry.service';

@Component({
  selector: 'app-time-entry-list',
  templateUrl: './time-entry-list.component.html',
  styleUrls: ['./time-entry-list.component.css']
})
export class TimeEntryListComponent implements OnInit {
  timeEntries: any[] = [];

  constructor(private timeEntryService: TimeEntryService) { }

  ngOnInit(): void {
    this.timeEntryService.getTimeEntries().subscribe(data => {
      this.timeEntries = data;
    });
  }
}

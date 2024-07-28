import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

// Import components
import { PeopleListComponent } from './components/people-list/people-list.component';
import { TimeEntryFormComponent } from './components/time-entry-form/time-entry-form.component';
import { TimeEntryListComponent } from './components/time-entry-list/time-entry-list.component';
import { WorkTaskListComponent } from './components/work-task-list/work-task-list.component';

// Import services
import { PeopleService } from './services/people.service';
import { TimeEntryService } from './services/time-entry.service';
import { WorkTaskService } from './services/work-task.service';

@NgModule({
  declarations: [
    AppComponent,
    PeopleListComponent,
    TimeEntryFormComponent,
    TimeEntryListComponent,
    WorkTaskListComponent
  ],
  imports: [
    BrowserModule,
    FormsModule, 
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [
    PeopleService,
    TimeEntryService,
    WorkTaskService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

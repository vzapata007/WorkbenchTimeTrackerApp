import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbModule, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap'; 

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
    ReactiveFormsModule,
    HttpClientModule,
    AppRoutingModule,
    NgbModule 
  ],
  providers: [
    PeopleService,
    TimeEntryService,
    WorkTaskService,
    NgbActiveModal 
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

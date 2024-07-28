import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PeopleListComponent } from './components/people-list/people-list.component';
import { WorkTaskListComponent } from './components/work-task-list/work-task-list.component';
import { TimeEntryListComponent } from './components/time-entry-list/time-entry-list.component';
import { TimeEntryFormComponent } from './components/time-entry-form/time-entry-form.component';

const routes: Routes = [
  { path: 'people', component: PeopleListComponent },
  { path: 'work-tasks', component: WorkTaskListComponent },
  { path: 'time-entries', component: TimeEntryListComponent },
  { path: 'add-time-entry', component: TimeEntryFormComponent },
  { path: '', redirectTo: '/time-entries', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

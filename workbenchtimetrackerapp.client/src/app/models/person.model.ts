import { TimeEntry } from './time-entry.model';
import { WorkTask } from './work-task.model';

// Interface representing a person
export interface Person {
  id?: number; 
  fullName: string; 
  timeEntries?: TimeEntry[]; // List of time entries associated with the person
  workTasks?: WorkTask[]; // List of work tasks associated with the person
}

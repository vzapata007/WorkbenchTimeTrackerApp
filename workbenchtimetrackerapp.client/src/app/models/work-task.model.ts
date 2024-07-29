import { TimeEntry } from './time-entry.model';

// Interface representing a work task
export interface WorkTask {
  id?: number; 
  name: string; 
  description: string; 
  timeEntries?: TimeEntry[]; // Optional list of time entries associated with the work task
}

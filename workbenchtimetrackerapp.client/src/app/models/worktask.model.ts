import { TimeEntry } from './time-entry.model';

export class WorkTask {
  id?: number;
  name: string;
  description: string;
  timeEntries?: TimeEntry[];

  constructor(name: string, description: string, timeEntries?: TimeEntry[], id?: number) {
    this.name = name;
    this.description = description;
    this.timeEntries = timeEntries;
    this.id = id;
  }
}

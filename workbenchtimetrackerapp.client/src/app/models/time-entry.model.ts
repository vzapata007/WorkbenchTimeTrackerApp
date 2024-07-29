// Interface representing a time entry
export interface TimeEntry {
  id?: number;
  entryDateTime: Date;
  personId: number;
  workTaskId: number;
  personFullName?: string;  
  workTaskName?: string;
}

export class TimeEntry {
  id?: number;
  entryDateTime: Date;
  personId: number;
  workTaskId: number;

  constructor(entryDateTime: Date, personId: number, workTaskId: number, id?: number) {
    this.entryDateTime = entryDateTime;
    this.personId = personId;
    this.workTaskId = workTaskId;
    this.id = id;
  }
}

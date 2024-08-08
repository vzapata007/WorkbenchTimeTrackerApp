export * from './people.service';
import { PeopleService } from './people.service';
export * from './timeEntries.service';
import { TimeEntriesService } from './timeEntries.service';
export * from './weatherForecast.service';
import { WeatherForecastService } from './weatherForecast.service';
export * from './workTasks.service';
import { WorkTasksService } from './workTasks.service';
export const APIS = [PeopleService, TimeEntriesService, WeatherForecastService, WorkTasksService];

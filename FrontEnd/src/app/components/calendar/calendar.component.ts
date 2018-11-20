import { Component, OnInit } from '@angular/core';
import { SportsService } from 'src/app/services/sports.service';
import { Sport } from 'src/app/classes/sport';
import {
  ChangeDetectionStrategy,
  ViewChild,
  TemplateRef
} from '@angular/core';
import {
  startOfDay,
  endOfDay,
  subDays,
  addDays,
  isSameDay,
  isSameMonth
} from 'date-fns';
import { Subject } from 'rxjs';
import {
  CalendarEvent,
  CalendarView
} from 'angular-calendar';
import { Encounter } from 'src/app/classes/encounter';
import { LoginService } from 'src/app/services/login/login.service';

const colors: any = {
  red: {
    primary: '#ad2121',
    secondary: '#FAE3E3'
  },
  blue: {
    primary: '#1e90ff',
    secondary: '#D1E8FF'
  },
  yellow: {
    primary: '#e3bc08',
    secondary: '#FDF1BA'
  }
};

@Component({
  selector: 'app-calendar',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.css']
})
export class CalendarComponent implements OnInit {

  constructor(
    private loginService: LoginService,
    private sportsService: SportsService) { }

  @ViewChild('modalContent')
  modalContent: TemplateRef<any>;

  isAdmin: boolean;
  view: CalendarView = CalendarView.Month;
  CalendarView = CalendarView;
  viewDate: Date = new Date();

  refresh: Subject<any> = new Subject();

  events: CalendarEvent[] = [];

  activeDayIsOpen = false;

  selectedSport: string;
  sports: Array<Sport>;
  encounters: Array<Encounter>;

  dayClicked({ date, events }: { date: Date; events: CalendarEvent[] }): void {
    if (isSameMonth(date, this.viewDate)) {
      this.viewDate = date;
      if (
        (isSameDay(this.viewDate, date) && this.activeDayIsOpen === true) ||
        events.length === 0
      ) {
        this.activeDayIsOpen = false;
      } else {
        this.activeDayIsOpen = true;
      }
    }
  }

  addEvent(): void {
    this.events.push({
      title: 'New event',
      start: startOfDay(new Date()),
      end: endOfDay(new Date()),
      color: colors.red,
      draggable: true,
      resizable: {
        beforeStart: true,
        afterEnd: true
      }
    });
    this.refresh.next();
  }

  ngOnInit() {
    this.isAdmin = (this.loginService.getLoggedUserRole() === 'Administrator');
    this.getSportData();
  }

  private getSportData() {
    this.sportsService.getSports().subscribe(
      ((data: Array<Sport>) => { this.sports = data; }),
      ((error: any) => console.log(error))
    );
  }

  onSelectSport() {
    this.getEncountersBySportData();
  }

  private getEncountersBySportData() {
    console.log('Sport ' + this.selectedSport);
    this.sportsService.getEncountersBySport(this.selectedSport).subscribe(
      ((data: Array<Encounter>) => {
        this.encounters = data;
        this.loadCalendar();
      }),
      ((error: any) => console.log(error))
    );
  }

  private loadCalendar() {
    this.events = [];
    this.encounters.forEach(encounter => {
      this.events.push({
        start: subDays(startOfDay(encounter.dateTime), 0),
        end: addDays(encounter.dateTime, 0),
        title: this.getInformation(encounter),
        color: colors.red,
        allDay: true,
      });
    });
    this.refresh.next();
  }

  private getInformation(encounter: Encounter): string {
    const teamsInEncounter = encounter.teamIds;
    let i = 1;
    let singleStringTeams = '';
    teamsInEncounter.forEach(teamId => {
      const teamName = teamId.split('_');

      if (i === teamsInEncounter.length) {
        singleStringTeams += teamName[0];
      } else {
        singleStringTeams += teamName[0] + ' vs ';
      }

      i++;
    });

    const convertedStartDate = new Date(encounter.dateTime);
    const month = convertedStartDate.getMonth() + 1;
    const date = convertedStartDate.getDate();
    const year = convertedStartDate.getFullYear();
    const shortStartDate = date + '-' + month + '-' + year;

    return '<br> <strong>Deporte:</strong> ' + encounter.sportName
      + '<br> <strong>Equipos:</strong> ' + singleStringTeams
      + '<br> <strong>Fecha:</strong> ' + shortStartDate;
  }
}

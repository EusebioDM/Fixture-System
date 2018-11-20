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
  endOfMonth,
  isSameDay,
  isSameMonth,
  addHours
} from 'date-fns';
import { Subject } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import {
  CalendarEvent,
  CalendarEventAction,
  CalendarEventTimesChangedEvent,
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
    private sportsService: SportsService,
    private modal: NgbModal
  ) { }

  @ViewChild('modalContent')
  modalContent: TemplateRef<any>;

  isAdmin: boolean;
  view: CalendarView = CalendarView.Month;
  CalendarView = CalendarView;
  viewDate: Date = new Date();

  /*
  modalData: {
    action: string;
    event: CalendarEvent;
  };

  actions: CalendarEventAction[] = [
    {
      label: '<i class="fa fa-fw fa-pencil"></i>',
      onClick: ({ event }: { event: CalendarEvent }): void => {
        this.handleEvent('Edited', event);
      }
    },
    {
      label: '<i class="fa fa-fw fa-times"></i>',
      onClick: ({ event }: { event: CalendarEvent }): void => {
        this.events = this.events.filter(iEvent => iEvent !== event);
        this.handleEvent('Deleted', event);
      }
    }
  ];
  */

  refresh: Subject<any> = new Subject();

  events: CalendarEvent[] = [
    /*{
      start: subDays(startOfDay(new Date()), 1),
      end: addDays(new Date(), 1),
      title: 'A 3 day event',
      color: colors.red,
      actions: this.actions,
      allDay: true,
      resizable: {
        beforeStart: true,
        afterEnd: true
      },
      draggable: true
    },
    {
      start: startOfDay(new Date()),
      title: 'An event with no end date',
      color: colors.yellow,
      actions: this.actions
    },
    {
      start: subDays(endOfMonth(new Date()), 3),
      end: addDays(endOfMonth(new Date()), 3),
      title: 'A long event that spans 2 months',
      color: colors.blue,
      allDay: true
    },
    {
      start: addHours(startOfDay(new Date()), 2),
      end: new Date(),
      title: 'A draggable and resizable event',
      color: colors.yellow,
      actions: this.actions,
      resizable: {
        beforeStart: true,
        afterEnd: true
      },
      draggable: true
    }*/
  ];

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

  /*
  eventTimesChanged({
    event,
    newStart,
    newEnd
  }: CalendarEventTimesChangedEvent): void {
    event.start = newStart;
    event.end = newEnd;
    this.handleEvent('Dropped or resized', event);
    this.refresh.next();
  }
  */

  /*
  handleEvent(action: string, event: CalendarEvent): void {
    this.modalData = { event, action };
    this.modal.open(this.modalContent, { size: 'lg' });
  }
  */

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
        // actions: this.actions,
        allDay: true,
        // resizable: {
        //  beforeStart: true,
        //  afterEnd: true
        // },
        // draggable: true
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

    return '<br> <strong>Deporte:</strong> ' + encounter.sportName
      + '<br> <strong>Equipos:</strong> ' + singleStringTeams
      + '<br> <strong>Fecha:</strong> ' + encounter.dateTime;
  }
}

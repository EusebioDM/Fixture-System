import { Component, OnInit } from '@angular/core';
import { SportsService } from 'src/app/services/sports.service';
import { Sport } from 'src/app/classes/sport';

@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.css']
})
export class CalendarComponent implements OnInit {

  constructor(private sportsService: SportsService) { }

  sports: Array<Sport>;

  ngOnInit() {
    this.getData();
  }

  private getData() {
    this.sportsService.getSports().subscribe(
      ((data: Array<Sport>) => { this.sports = data; }),
      ((error: any) => console.log(error))
    );
  }

  onSelectSport() {

  }
}

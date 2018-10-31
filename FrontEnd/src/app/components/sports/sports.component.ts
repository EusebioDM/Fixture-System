import { Component, OnInit, ViewChild } from '@angular/core';
import { SportsService } from 'src/app/services/sports.service';
import { Sport } from 'src/app/classes/sport';
import { MatTableDataSource, MatPaginator } from '@angular/material';

@Component({
  selector: 'app-sports',
  templateUrl: './sports.component.html',
  styleUrls: ['./sports.component.css']
})
export class SportsComponent implements OnInit {

  constructor(private sportsService: SportsService) { }

  @ViewChild(MatPaginator) paginator: MatPaginator;

  displayedColumns: string[] = ['sportName'];
  dataSource;

  sports: Array<Sport>;

  ngOnInit() {
    this.sportsService.getSports().subscribe(
      ((data: Array<Sport>) => this.result(data)),
      ((error: any) => console.log(error))
    );
  }

  private result(data: Array<Sport>): void {
    this.sports = data;
    this.loadTableDataSource();
  }

  private loadTableDataSource() {
    this.dataSource = new MatTableDataSource<Sport>(this.sports);
    this.dataSource.paginator = this.paginator;
  }



}

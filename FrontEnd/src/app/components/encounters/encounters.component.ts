import { Component, OnInit, ViewChild } from '@angular/core';
import { EncountersService } from 'src/app/services/encounters.service';
import { MatSort, MatPaginator, MatTableDataSource } from '@angular/material';
import { Encounter } from 'src/app/classes/encounter';

@Component({
  selector: 'app-encounters',
  templateUrl: './encounters.component.html',
  styleUrls: ['./encounters.component.css']
})
export class EncountersComponent implements OnInit {

  constructor(
    private encounterServices: EncountersService
  ) { }

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  displayedColumns: string[] = ['sport', 'teams', 'date', 'actionDelete'];
  dataSource;
  searchKey: string;
  encounters: Array<Encounter>;

  ngOnInit() {
    this.encounterServices.getEncounters().subscribe(
      ((data: Array<Encounter>) => this.result(data)),
      ((error: any) => console.log(error))
    );
  }

  private result(data: Array<Encounter>): void {
    this.encounters = data;
    this.loadTableDataSource();
  }

  private loadTableDataSource() {
    this.dataSource = new MatTableDataSource<Encounter>(this.encounters);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

}

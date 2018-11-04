import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { SportsService } from 'src/app/services/sports.service';
import { Sport } from 'src/app/classes/sport';
import { MatTableDataSource, MatPaginator, MatDialogConfig, MatDialog, MatSort, MAT_DIALOG_DATA } from '@angular/material';
import { AddSportComponent } from '../add-sport/add-sport.component';

@Component({
  selector: 'app-sports',
  templateUrl: './sports.component.html',
  styleUrls: ['./sports.component.css']
})
export class SportsComponent implements OnInit {

  constructor(
    private sportsService: SportsService,
    private dialog: MatDialog
  ) { }

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  displayedColumns: string[] = ['sportName', 'actionDelete'];
  dataSource;
  searchKey: string;
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
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    /*
    this.dataSource.filterPredicate = (data, filter) => {
      return this.displayedColumns.some(ele => {
        return ele !== 'actionDelete' && data[ele].toLowerCase().indexOf(filter) !== -1;
      });
    };
    */
  }

  onSearchClear() {
    this.searchKey = '';
    this.applyFilter();
  }

  applyFilter() {
    this.dataSource.filter = this.searchKey.trim().toLowerCase();
  }

  onInsert() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    const dialogRef = this.dialog.open(AddSportComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(
      ((sport: Sport) => {
        if (sport !== undefined) {
            console.log('El deporte devuelto es: ' + sport.name);
            this.sports.push(sport);
            this.loadTableDataSource();
        }
      })
    );
  }

}

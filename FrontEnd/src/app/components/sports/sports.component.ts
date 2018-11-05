import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { SportsService } from 'src/app/services/sports.service';
import { Sport } from 'src/app/classes/sport';
import { MatTableDataSource, MatPaginator, MatDialogConfig, MatDialog, MatSort, MAT_DIALOG_DATA } from '@angular/material';
import { AddSportComponent } from '../add-sport/add-sport.component';
import { YesNoDialogComponent } from '../yes-no-dialog/yes-no-dialog.component';

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
          this.sports.push(sport);
          this.loadTableDataSource();
        }
      })
    );
  }

  onDelete(sportId: string) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    const dialogRef = this.dialog.open(YesNoDialogComponent, dialogConfig);
    dialogRef.componentInstance.title = 'Borrar deporte...';
    dialogRef.componentInstance.message = '¿Está seguro que quiere borrar al deporte ' + sportId + ' y todos sus equipos?';

    dialogRef.afterClosed().subscribe(
      ((result) => {
        if (result) {
          this.sportsService.deleteSport(sportId).subscribe();
          this.updateDataSource(sportId);
        }
      })
    );
  }

  private updateDataSource(id: string) {

    let sp;
    this.dataSource.data.forEach(sport => {
      if (sport.name === id) {
        sp = sport;
      }
    });

    this.dataSource.data.splice(this.dataSource.data.indexOf(sp), 1);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }
}

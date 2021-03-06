import { Component, OnInit, ViewChild } from '@angular/core';
import { Log } from '../../classes/log';
import { LogsService } from '../../services/logs.service';
import { MatTableDataSource, MatPaginator, MatDialog, MatSort, MatDialogConfig } from '@angular/material';

@Component({
  selector: 'app-logs',
  templateUrl: './logs.component.html',
  styleUrls: ['./logs.component.css']
})
export class LogsComponent implements OnInit {
  logs: Log[];
  dataSource;
  start: Date;
  end: Date;
  displayedColumns: string[] = ['dateTime', 'userName', 'action'];

  constructor(private logsService: LogsService) { }

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  ngOnInit() {
    this.getData();
  }

  private getData() {
    this.logsService.getLogs().subscribe(
      ((data: Array<Log>) => {
        this.logs = data;
        this.loadTableDataSource();
      }),
      ((error: any) => console.log(error))
    );
  }

  private loadTableDataSource() {
    this.dataSource = new MatTableDataSource<Log>(this.logs);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  onFilterByDate() {

    if (this.start && this.end) {
      const startMouth = this.start.getMonth() + 1;
      const endMouth = this.end.getMonth() + 1;

      const dateTo = this.start.getFullYear() + '-' + startMouth + '-' + this.start.getDate();
      const dateFor = this.end.getFullYear() + '-' + endMouth + '-' + this.end.getDate();

      this.logsService.getLogsFromToDate(dateTo, dateFor).subscribe(
        ((data: Array<Log>) => {
          this.logs = data;
          this.loadTableDataSource();
        }),
        ((error: any) => console.log(error))
      );
    } else {
      this.getData();
    }
  }
}

import { Component, OnInit } from '@angular/core';
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

  ngOnInit() {
    this.getData();
  }

  private getData() {
    this.logsService.getUsers().subscribe(
      ((data: Array<Log>) => {
        this.logs = data;
        this.dataSource = new MatTableDataSource<Log>(this.logs);
      }),
      ((error: any) => console.log(error))
    );
  }

}

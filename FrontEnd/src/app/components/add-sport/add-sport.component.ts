import { Component, OnInit, Inject } from '@angular/core';
import { SportsService } from 'src/app/services/sports.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { SportsComponent } from '../sports/sports.component';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { Sport } from 'src/app/classes/sport';
import { InstantErrorStateMatcher } from 'src/app/shared/instant-error-state-matcher';

@Component({
  selector: 'app-add-sport',
  templateUrl: './add-sport.component.html',
  styleUrls: ['./add-sport.component.css']
})
export class AddSportComponent implements OnInit {

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: AddSportComponent,
    private sportsService: SportsService,
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<SportsComponent>
  ) { }

  error: string;
  addSportForm: FormGroup;
  matcher = new InstantErrorStateMatcher();

  ngOnInit() {
    this.createAddSportForm();
  }

  createAddSportForm() {
    this.addSportForm = this.formBuilder.group({
      name: ['',
        Validators.required
      ],
      encounterPlayerCount: ['',
        Validators.required
      ]
    });
  }

  get name() {
    return this.addSportForm.get('name');
  }

  get encounterPlayerCount() {
    return this.addSportForm.get('encounterPlayerCount');
  }

  submit() {
    const sport = this.addSportForm.value;
    this.sportsService.addSport(sport).subscribe(
      ((result: Sport) => {
        this.dialogRef.close(sport);
      }),
      (error) => this.error = error
    );
  }
}

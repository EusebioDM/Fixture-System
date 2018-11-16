import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Sport } from '../../classes/sport';
import { SportsService } from '../../services/sports.service';
import { Fixture } from '../../classes/fixture';
import { EncountersService } from '../../services/encounters.service';
import { EncountersComponent } from '../encounters/encounters.component';
import { MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-generate-fixture',
  templateUrl: './generate-fixture.component.html',
  styleUrls: ['./generate-fixture.component.css']
})
export class GenerateFixtureComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<EncountersComponent>,
    private sportsService: SportsService,
    private encountersService: EncountersService,
    private formBuilder: FormBuilder
  ) { }

  generateFixtureForm: FormGroup;
  error: string;
  sports: Array<Sport>;
  fixtures: Array<string>;

  ngOnInit() {
    this.getSportsData();
    this.createGenerateFixtureForm();
  }

  getSportsData() {
    this.sportsService.getSports().subscribe(
      ((data: Array<Sport>) => { this.sports = data; }),
      ((error: any) => console.log(error))
    );
  }

  getAvailableFixtures() {
    this.encountersService.GetAvailableFixtureGenerators().subscribe(
      ((data: Array<string>) => { this.fixtures = data; }),
      ((error: any) => console.log(error))
    );
  }

  createGenerateFixtureForm() {
    this.generateFixtureForm = this.formBuilder.group({
      startingDate: ['',
        Validators.required
      ],
      sportName: ['',
        Validators.required
      ],
      creationAlgorithmName: ['',
        Validators.required
      ],
    });
  }

  get startingDate() {
    return this.generateFixtureForm.get('startingDate');
  }

  get sportName() {
    return this.generateFixtureForm.get('sportName');
  }

  get creationAlgorithmName() {
    return this.generateFixtureForm.get('creationAlgorithmName');
  }

  submit() {
    const fixture = this.generateFixtureForm.value;
    this.encountersService.createFixture(fixture).subscribe(
      (() => {
        this.dialogRef.close(true);
      }),
      (error) => this.error = error
    );
  }
}

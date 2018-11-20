import { TeamResult } from './team-result';

export class Encounter {
    id: string;
    dateTime: Date;
    teamIds: Array<string>;
    sportName: string;
    results: Array<TeamResult>;

    constructor(id: string, dateTime: Date, teamIds: Array<string>, sportName: string, results: Array<TeamResult>) {
        this.id = id;
        this.dateTime = dateTime;
        this.teamIds = teamIds;
        this.sportName = sportName;
        this.results = results;
    }
}

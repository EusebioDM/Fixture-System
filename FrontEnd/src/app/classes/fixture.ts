export class Fixture {
    creationAlgorithmName: string;
    sportName: string;
    startingDate: string;

    constructor(creationAlgorithmName: string, sportName: string, startingDate: string) {
        this.creationAlgorithmName = creationAlgorithmName;
        this.sportName = sportName;
        this.startingDate = startingDate;
    }
}

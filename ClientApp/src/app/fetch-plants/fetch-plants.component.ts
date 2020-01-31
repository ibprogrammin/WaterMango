import { Component, Inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { start } from 'repl';

@Injectable()
@Component({
  selector: 'app-fetch-plants',
  templateUrl: './fetch-plants.component.html'
})
export class FetchPlantsComponent {
  private baseUrl: string;
  public plants: Plant[];
  public cantWater: boolean;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.cantWater = false;

    this.loadPlants(-1);
  }

  waterPlant(plantId: number) {
    var waterUrl = this.baseUrl + 'Plant/' + plantId + '/water';
    this.startWatering(waterUrl, plantId);
  }

  private interval: NodeJS.Timeout;
  private lastPlantWatered: number;

  //Starts the watering routine on the selected plant
  startWatering(requestUrl: string, plantId: number) {
    if (this.lastPlantWatered != plantId) {
      this.resetPlantsWatering();
    }
    this.cantWater = false;
    this.lastPlantWatered = plantId;  
    this.pauseWatering();

    if (document.getElementById('waterPlantButton' + plantId).getAttribute('value') == 'Start') {
      this.interval = setInterval(() => {
        this.http.put(requestUrl, "").subscribe(response => {
          switch (response) {
            case 0: {
              this.pauseWatering();
              this.loadPlants(-1);
            }
            case -1: {
              this.cantWater = true;              
              this.pauseWatering();
              this.setPlantWatering(plantId, false);
            }
            default: {
              this.loadPlants(plantId);
            }
          }
        }, error => console.error(error));
      }, 1000);
    }
    else {
      this.pauseWatering();
      this.setPlantWatering(plantId, false);
    }
    
  }

  //Performs an http get call to retrieve all the plants
  loadPlants(plantId: number) {
    this.http.get<Plant[]>(this.baseUrl + 'plant').subscribe(result => {
      this.plants = result;
      if (plantId >= 0) { this.setPlantWatering(plantId, true); }
      else { this.resetPlantsWatering(); }
    }, error => console.error(error));
  }

  //Pauses watering of plants
  pauseWatering() {
    clearInterval(this.interval);
  }

  //Sets a plants watering flag
  setPlantWatering(plantId: number, watering: boolean) {
    if (this.plants.length !== 0) {
      this.plants[this.getPlantIndex(plantId)].watering = watering;
    }
  }

  //Resets all plants watering flag to false
  resetPlantsWatering() {
    if (this.plants.length !== 0) {
      for (let plant of this.plants) {
        plant.watering = false;
      }
    }
  }

  //Gets the array index of the plant that has a matching Plant Id
  getPlantIndex(plantId: number): number {
    if (this.plants.length !== 0) {
      for (let plant of this.plants) {
        if (plant.id === plantId) {
          return this.plants.indexOf(plant);
        }
      }
    }
    else { return -1; }
  }

}

//Plant Object Interface
interface Plant {
  id: number;
  name: string;
  lastUpdated: string;
  secondsWatered: number;
  watered: boolean;
  watering: boolean;
}

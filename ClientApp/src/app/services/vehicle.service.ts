import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs';
import { SaveVehicle, Vehicle } from '../interfaces/vehicle';

@Injectable({
  providedIn: 'root'
})
export class VehicleService {
  private readonly vehiclesEndpoint = 'https://localhost:7022/api/vehicles/';
  constructor(private http: HttpClient) { }

  getMakes() {
    return this.http.get('https://localhost:7022/api/makes')
      .pipe(map(obj => [...Object.values(obj)]));      
  }

  getFeatures() {
    return this.http.get('https://localhost:7022/api/features')
      .pipe(map(obj => [...Object.values(obj)]));
  }

  create(vehicle: any) {
    return this.http.post(this.vehiclesEndpoint, vehicle);      
  }

  getVehicle(id: number) {   
      return this.http.get(this.vehiclesEndpoint + id);  
  }

  update(vehicle: SaveVehicle) {
    return this.http.put(this.vehiclesEndpoint + vehicle.id, vehicle);
  }

  delete(id: number) {
    return this.http.delete(this.vehiclesEndpoint + id);
  }

  

  getVehicles(filter: string) {
    return this.http.get(this.vehiclesEndpoint + '?' + this.toQueryString(filter));
  }

  toQueryString(obj:any) {
    var parts = [];
    for(var property in obj) {
      var value = obj[property];
      if(value != null && value != undefined)
        parts.push(encodeURIComponent(property) + '=' + encodeURIComponent(value));
    }

    return parts.join('&');
  }
}

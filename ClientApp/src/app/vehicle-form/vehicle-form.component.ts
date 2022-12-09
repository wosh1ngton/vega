import * as _ from 'underscore';
import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin, Observable, of } from 'rxjs';
import { SaveVehicle, Vehicle } from '../interfaces/vehicle';
import { VehicleService } from '../services/vehicle.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-vehicle-form',
  templateUrl: './vehicle-form.component.html',
  styleUrls: ['./vehicle-form.component.css']
})
export class VehicleFormComponent implements OnInit {
  makes: any[] = [];
  models: any[] = [];
  vehicle: SaveVehicle = {
    id: 0,
    makeId: 0,
    modelId: 0,
    isRegistered: false,
    features: [],
    contact: {
      name: '',
      email: '',
      phone: '',
    }
  };
  features: any[] = [];


  constructor(
    private vehicleService: VehicleService,
    private route: ActivatedRoute,
    private toastrService: ToastrService,
    private router: Router) {
    route.params.subscribe(p => {
      this.vehicle.id = +p['id'] || 0;
    })
  }

  ngOnInit(): void {

    var sources: Observable<any>[] = [
      this.vehicleService.getMakes(),
      this.vehicleService.getFeatures(),
    ];

    if (this.vehicle.id)
      sources.push(this.vehicleService.getVehicle(this.vehicle.id));

    forkJoin(sources).subscribe({
      next: (v) => {
        this.makes = v[0];
        this.features = v[1];
        if (this.vehicle.id) {
          this.setVehicle(v[2]);
          this.populatedModels();
        }
      },
      error: (e) => {
        if (e.status == 404)
          this.router.navigate(['/home']);
      }
    });

  }
  private setVehicle(v: Vehicle) {
    this.vehicle.id = v.id;
    this.vehicle.makeId = v.make.id;
    this.vehicle.modelId = v.model.id;
    this.vehicle.isRegistered = v.isRegistered;
    this.vehicle.contact = v.contact;
    this.vehicle.features = _.pluck(v.features, 'id');
  }

  onMakeChange() {
    this.populatedModels();
    delete this.vehicle.modelId;
  }

  private populatedModels() {
    var selectedMake = this.makes.find(m => m.id == this.vehicle.makeId);
    this.models = selectedMake ? selectedMake.models : [];
  }
  onFeatureToggle(featureId: number, $event: any) {
    if ($event.target.checked)
      this.vehicle.features.push(featureId);
    else {
      var index = this.vehicle.features.indexOf(featureId);
      this.vehicle.features.splice(index, 1);
    }
  }
  // .pipe(
  //   map(heroes => heroes[0]),
  //   tap(h => {
  //     const outcome = h ? 'fetched' : 'did not find';
  //     this.log(`${outcome} hero id=${id}`);
  //   }),
  //   catchError(this.handleError<Hero>(`getHero id=${id}`))
  // );
  submit() {
    
    var result$ = (this.vehicle.id) ? this.vehicleService.update(this.vehicle) : this.vehicleService.create(this.vehicle);
    console.log(this.vehicle + 'ola');
    result$.subscribe(vehicle => {
      next: (vehicle:any) => {
       // this.toastrService.success('sucesso!');
        console.log("oi" + vehicle);
        this.router.navigate(['/vehicles/', vehicle.id]);        
      };
      
    });  
   
  }


  delete() {
    if (confirm("Are you sure?")) {
      this.vehicleService.delete(this.vehicle.id)
        .subscribe({
          next: (x) => {
            this.router.navigate(['/home']);
          }
        });
    }
  }
}


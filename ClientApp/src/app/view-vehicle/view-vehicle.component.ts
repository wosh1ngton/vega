import { HttpEventType } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs';
import { PhotoService } from '../services/photo.service';
import { VehicleService } from '../services/vehicle.service';

@Component({
  selector: 'app-view-vehicle',
  templateUrl: './view-vehicle.component.html',
  styleUrls: ['./view-vehicle.component.css']
})
export class ViewVehicleComponent implements OnInit {
  vehicle:any;
  vehicleId: number = 0;
  barWidth:string="0%";  
  @ViewChild('fileInput') fileInput!: ElementRef; 
  photos: any;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private photoService: PhotoService,
    //private toastService:ToastrService,
    private vehicleService: VehicleService
  ) { 
    route.params.subscribe(p => {
      this.vehicleId = +p['id'];
      if(isNaN(this.vehicleId) || this.vehicleId <= 0) {
        router.navigate(['/vehicles']);
        return;
      }
    });
  }

  ngOnInit(): void {
    this.photoService.getPhotos(this.vehicleId)
      .subscribe(photos => this.photos = photos);

    this.vehicleService.getVehicle(this.vehicleId)
      .subscribe({
        next: (v) => this.vehicle = v,
        error: (e) => {
          if(e.status == 404) {            
            this.router.navigate(['/vehicles']);
            return;
          }
        }
      })
  }

  delete() {
    if(confirm("Are you sure?")) {
      this.vehicleService.delete(this.vehicle.id)
        .subscribe(x => {
          this.router.navigate(['/vehicles']);
        })
    }
  }


  deletePhoto(id: number) {
    if(confirm("Are you sure?")) {
      this.photoService.delete(this.vehicle.id, id)
        .subscribe(x => {
          this.router.navigate(['/vehicles/' + this.vehicle.id]);
        })
    }
  }

//   .pipe(map(
//     event => {
//         if(event.type == HttpEventType.UploadProgress)
//         {
//             this.barWidth = (Math.round((100/ (event.total || 0) * event.loaded))).toString()
//         }
//         else if (event.type == HttpEventType.Response) {
//             this.barWidth = "0%";  
//             console.log("upload completo")                                      
//         }
//     }
// ));

  uploadPhoto() 
  {
    var nativeElement: HTMLInputElement = this.fileInput.nativeElement;

    this.photoService.upload(this.vehicleId, nativeElement.files![0])
      .pipe(map(
            event => {
                if(event.type == HttpEventType.UploadProgress)
                {
                    this.barWidth = (Math.round((100/ (event.total || 0) * event.loaded))).toString()
                    console.log(this.barWidth);
                }
                else if (event.type == HttpEventType.Response) {
                    this.barWidth = "0%";  
                    console.log("upload completo")  ;                                    
                }
            }
        ))
      .subscribe(x => this.photos.push(x))
  }

}

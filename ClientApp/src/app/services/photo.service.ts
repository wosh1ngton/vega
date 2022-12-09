import { HttpClient, HttpEventType } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';

@Injectable({
    providedIn:'root'
})
export class PhotoService {
    /**
     *
     */
    
    private readonly vehiclesEndpoint = 'https://localhost:7022/api/vehicles/';
    constructor(private http: HttpClient) {   
    }


    // downfile(file: any): Observable<HttpEvent<any>>{

    //     return this.http.post(this.url , app, {
    //       responseType: "blob", reportProgress: true, observe: "events", headers: new HttpHeaders(
    //         { 'Content-Type': 'application/json' },
    //       )
    //     });
    //   }

    upload(vehicleId:number, photo:any) {
        var formData = new FormData();
        formData.append('file', photo);
        return this.http.post(this.vehiclesEndpoint + vehicleId + '/photos', formData, {
            reportProgress: true,
            observe: "events",
            responseType: 'blob'
        });
    }

    getPhotos(vehicleId:number) {
        return this.http.get(this.vehiclesEndpoint + vehicleId + '/photos');
    }

    delete(vehicleId: number, photoId: number) {
        return this.http.delete(this.vehiclesEndpoint + vehicleId + '/photos/' + photoId);
    }
    
}
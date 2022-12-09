import { ErrorHandler, Inject, Injectable, Injector, NgZone, isDevMode } from "@angular/core";
import { ToastrService } from "ngx-toastr";

// @Injectable({
//     providedIn: 'root'
//   })
@Injectable()
export class AppErrorHandler implements ErrorHandler  {

    
    /**
     *
     */
    constructor(private injector: Injector, private ngZone: NgZone) {}
    
    //constructor(private toastyService: ToastrService) { }
    /**
     *
     */
    //constructor(private toastr: ToastrService) { }
   
    handleError(error: any): void {
        
        const toastyService = this.injector.get(ToastrService);
        this.ngZone.run((e) => {
            console.log("Erro:" + e);
            toastyService.error('teste');
        })     
       
    }
    /**
     *
     */
    
}
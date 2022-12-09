import { Component, Inject, inject, OnInit, ViewChild } from '@angular/core';
import { ToastContainerDirective, ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  
  
   //constructor(@Inject(ToastrService) private toastr: ToastrService) { }
   constructor(private toastr: ToastrService) { }
  //  @ViewChild(ToastContainerDirective, {static: true})
  //  toastContainer: ToastContainerDirective | undefined;

  /**
   *
   */
  

  ngOnInit(): void {
     //this.toastr.overlayContainer = this.toastContainer;
  }
  onClick() {
    this.toastr.error('Clicou errado, trouxa');
  }
  title = 'app';
}

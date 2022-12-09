import { Component, OnInit } from '@angular/core';
import { KeyValuePair, Vehicle } from '../interfaces/vehicle';
import { VehicleService } from '../services/vehicle.service';

@Component({
  selector: 'app-vehicle-list',
  templateUrl: './vehicle-list.component.html',
  styleUrls: ['./vehicle-list.component.css']
})
export class VehicleListComponent implements OnInit {
  private readonly PAGE_SIZE = 3;
  queryResult: any = {
    totalItems: 0,
    items:[]
  };  
  makes: KeyValuePair[] = [];
  query: any = {    
    pageSize: this.PAGE_SIZE
  };
  columns = [
    {title: 'Id'},
    {title: 'Contact Name', key: 'contactName', isSortable: true},
    {title: 'Make', key: 'make', isSortable: true},
    {title: 'Model', key: 'model', isSortable: true},
    {}
  ];

  constructor(private vehicleService: VehicleService) { }

  ngOnInit(): void {
    this.vehicleService.getMakes()
      .subscribe(makes => this.makes = makes);

    this.populatedVehicles();     
  }

  private populatedVehicles() {
    this.vehicleService.getVehicles(this.query)
      .subscribe(result => {
        this.queryResult = result, console.log(this.queryResult)
      });
  }

  onFilterChange() {
    this.query.page= 1;    
    this.populatedVehicles();
  }

  resetFilter() {
    this.query = {
      page:1,
      pageSize: this.PAGE_SIZE
    };
    this.populatedVehicles();
  }

  sortBy(columnName:string | undefined) {
    if(this.query.sortBy === columnName) {
      this.query.isSortAscending = !this.query.isSortAscending;
    } else {
      this.query.sortBy = columnName;
      this.query.isSortAscending = true;
    }
    this.populatedVehicles();
  }

  onPageChange(page:number) {
    this.query.page=page;
    this.populatedVehicles();
  }
}

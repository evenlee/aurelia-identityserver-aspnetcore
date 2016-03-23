import {inject} from 'aurelia-framework';
import {HttpClient} from 'aurelia-fetch-client';
import 'isomorphic-fetch';
import config from './authConfig';

@inject(HttpClient)
export class Customer{
    heading = 'Customer management';
    customer=[];
    constructor(http){
        this.http = http;
    }

    activate(){
        let url = config.apiServerBaseAddress + ':57391/api/Customers';
        return this.http.fetch(url)
		.then(response => {
		    return response.json();
		})
		.then(c => {
		    return this.customers = c;
		});
    }

}

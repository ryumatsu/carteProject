import axios from 'axios';
import * as helper from '../services/serviceHelpers';

const endpoint = `${helper.API_HOST_PREFIX}/api/licenses`;

const addLicense = (payload) => {
    const config = {
        method: 'POST',
        url: endpoint,
        data: payload,
        crossdomain: true,
        headers: { 'Content-Type': 'application/json' },
    };
    return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};

const callLicenseTable = (pageIndex, pageSize) => {
    const config = {
        method: 'GET',
        url: endpoint + `/?pageIndex=${pageIndex}&&pageSize=${pageSize}`,
        crossdomain: true,
        headers: { 'Content-Type': 'application/json' },
    };
    return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};

const queryLicenseTable = (pageIndex, pageSize, query) => {
    const config = {
        method: 'GET',
        url: endpoint + `/query/?pageIndex=${pageIndex}&&pageSize=${pageSize}&&query=${query}`,
        crossdomain: true,
        headers: { 'Content-Type': 'application/json' },
    };
    return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};
const licenseServices = { addLicense, callLicenseTable, queryLicenseTable };

export default licenseServices;

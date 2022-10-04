import React from 'react';
import { propTypes } from 'react-bootstrap/esm/Image';
import './license.css';
import LicensesTableList from './LicensesTableList';
import AddLicensesForm from './AddLicensesForm';

function LicensesAdminPage() {
    return (
        <React.Fragment>
            <div className="license-form-container" key={propTypes.Id}>
                <AddLicensesForm></AddLicensesForm>
            </div>
            <div className="license-table-container" key={propTypes.Id}>
                <LicensesTableList></LicensesTableList>
            </div>
        </React.Fragment>
    );
}

export default LicensesAdminPage;

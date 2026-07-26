// src/CohortDetails.js
import React from 'react';
import styles from './CohortDetails.module.css';

function CohortDetails({ cohort }) {
  // Check if status is "ongoing" (case-insensitive)
  const isOngoing = cohort.status?.toLowerCase() === 'ongoing';

  return (
    <div className={styles.box}>
      {/* Dynamic inline style: green for ongoing, blue for others */}
      <h3 style={{ color: isOngoing ? 'green' : 'blue' }}>
        {cohort.code} - {cohort.technology}
      </h3>

      <dl>
        <dt>Started On</dt>
        <dd>{cohort.startDate}</dd>

        <dt>Current Status</dt>
        <dd>{cohort.status}</dd>

        <dt>Coach</dt>
        <dd>{cohort.coach}</dd>

        <dt>Trainer</dt>
        <dd>{cohort.trainer}</dd>
      </dl>
    </div>
  );
}

export default CohortDetails;
import { useState } from "react";
import PropTypes from 'prop-types';
import style from "./Tags.module.css"

function Tags(props) {
    const [canEdit, setCanEdit] = useState(props.canEdit);

    const onKeyUpAddTag = (event) => {
        if (event.key === 'Enter' && event.target.value.length > 0) {
            const tagExists = props.tags.find(t => t === event.target.value);

            if (!tagExists) {
                props.setShareTags([
                    ...props.tags,
                    event.target.value
                ]);
                event.target.value = "";
            }
        }
    }

    const onKeyDown = (event) => {
        if (event.keyCode === 13) {
            event.preventDefault();
            return false;
        }
    }

    const onClickAddTag = (event) => {
        if (event.detail === 1 && event.target.value.length > 0) {
            const tagExists = props.tags.find(t => t === event.target.value);

            if (!tagExists) {
                props.setShareTags([
                    ...props.tags,
                    event.target.value
                ]);
                event.target.value = "";
            }
        }
    }

    const deleteTag = (tagText) => {
        const tagsFiltered = props.tags.filter(t => t !== tagText);
        props.setShareTags(tagsFiltered);
    }

    return (
        <div className={style.container}>
            <label>Tagi</label>
            <div className={style.tagContainer}>
                {props.tags.map(t => (
                    <div className={style.tag} key = {t}>
                        <span>{t}</span>
                        {canEdit ? <span className={style.xButton}
                              onClick={() => deleteTag(t)}>x</span>
                              : null}
                    </div>
                ))}
                { canEdit ? 
                <input onKeyUp={onKeyUpAddTag} 
                       onClick={onClickAddTag}
                       onKeyDown={onKeyDown} />
                       : null }
            </div>
        </div>
    )
}

export default Tags;

Tags.defaultProps = {
    canEdit: true
}

Tags.propTypes = {
    tags: PropTypes.arrayOf(PropTypes.string).isRequired,
    setShareTags: PropTypes.func
}